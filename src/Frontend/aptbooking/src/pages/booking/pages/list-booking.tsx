import { useEffect, useState } from "react";
import filterService from "../../../utils/filter.service";
import { PaginationFilter } from "../../../utils/types/pagination-filter.interface";
import axiosInstance from "../../../utils/api.service";
import { APIs } from "../../../utils/common/api-paths";
import { DataTable, DataTableSortStatus } from "mantine-datatable";
import { ActionIcon, TextInput } from "@mantine/core";
import SearchIcon from "../../../components/Shared/Icons/search-icon";
import CloseIcon from "../../../components/Shared/Icons/close-icon";
import { dataTableProps } from "../../../utils/common/constants";
import { useDebouncedValue } from "@mantine/hooks";
import UnbookModal from "../components/unbook-apartment";
import messageService from "../../../utils/message.service";

const Bookings = () => {
  const PAGE_SIZES = dataTableProps.PAGE_SIZES;
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(PAGE_SIZES[0]);
  const [params, setParams] = useState(filterService.defaultFilter);
  const [recordsData, setRecordsData] = useState({ data: [], all_counts: 0 });
  const [sortStatus, setSortStatus] = useState<DataTableSortStatus>({
    columnAccessor: "apartmentName",
    direction: "asc",
  });
  const [apartmentNameFilter, setApartmentNameFilter] = useState("");
  const [debouncedApartmentNameFilter] = useDebouncedValue(
    apartmentNameFilter,
    550
  );
  const [manageBookingId, setManageBookingId] = useState<string>("");
  const [isUnbookModal, setIsUnbookModal] = useState<any>(false);

  useEffect(() => {
    const filterarray: any[] = [];
    if (debouncedApartmentNameFilter) {
      filterarray.push({
        Field: "apartmentName",
        Value: debouncedApartmentNameFilter,
        Operator: "contain",
      });
    }

    setParams({
      PageNumber: page,
      PageSize: pageSize,
      OrderBy: [sortStatus.columnAccessor + " " + sortStatus.direction],
      AdvancedFilter:
        filterarray.length == 0
          ? null
          : {
              Logic: "and",
              Filters: filterarray,
            },
    });
  }, [sortStatus, page, pageSize, debouncedApartmentNameFilter]);

  useEffect(() => {
    bindBookings(params);
  }, [params]);

  const bindBookings = async (params: PaginationFilter) => {
    const response = await axiosInstance.post(APIs.searchBookingApi, {
      PaginationFilter: params,
    });
    if (response.data) {
      setRecordsData({
        data: response.data.data,
        all_counts: response.data.totalCount,
      });
    }
  };

  const checkoutApartmentConfirm = (id: string) => {
    setManageBookingId(id);
    setIsUnbookModal(true);
  };

  const cancelBooking = async () => {
    const booking = await axiosInstance.get(
      APIs.getBookingApi + manageBookingId
    );

    if (booking.data.data.isOnLease === true) {
      const response = await axiosInstance.post(APIs.cancelLeaseApi, {
        bookingId: manageBookingId,
      });

      if (response.data) {
        messageService.showMessage(response.data.data);
        bindBookings(params);
        setIsUnbookModal(false);
      }
    } else {
      const response = await axiosInstance.post(APIs.cancelBookingApi, {
        bookingId: manageBookingId,
      });

      if (response.data) {
        messageService.showMessage(response.data.data);
        bindBookings(params);
        setIsUnbookModal(false);
      }
    }
  };

  return (
    <div className="panel">
      <div className="flex md:items-center md:flex-row flex-col mb-5 gap-5">
        <h5 className="font-semibold text-lg dark:text-white-light">
          Bookings
        </h5>
        <div className="ltr:ml-auto rtl:mr-auto"></div>
      </div>
      <div className="datatables">
        <DataTable
          highlightOnHover
          className="whitespace-nowrap table-hover"
          records={recordsData.data}
          columns={[
            {
              accessor: "apartmentName",
              title: "Apartment Name",
              filter: (
                <TextInput
                  label="Apartment Name"
                  description="Show apartments whose names include the specified text"
                  placeholder="Search apartments name..."
                  leftSection={<SearchIcon />}
                  rightSection={
                    <ActionIcon
                      size="sm"
                      variant="transparent"
                      c="dimmed"
                      onClick={() => setApartmentNameFilter("")}
                    >
                      <CloseIcon size={24} />
                    </ActionIcon>
                  }
                  value={apartmentNameFilter}
                  onChange={(e) =>
                    setApartmentNameFilter(e.currentTarget.value)
                  }
                />
              ),
              filtering: apartmentNameFilter !== "",
              sortable: true,
            },
            {
              accessor: "bookFromDisplay",
              title: "Book From",
              sortable: true,
            },
            { accessor: "bookTillDisplay", title: "Book Till", sortable: true },
            {
              accessor: "action",
              title: "Action",
              titleClassName: "!text-center",
              render: ({ id, isBook }) => (
                <div className="flex items-center w-max mx-auto gap-2">
                  {isBook === true && (
                    <button
                      type="button"
                      className="btn btn-outline-primary btn-sm"
                      onClick={() => checkoutApartmentConfirm(`${id}`)}
                    >
                      Cancel
                    </button>
                  )}
                  {isBook === false && (
                    <button
                      type="button"
                      className="btn btn-primary btn-sm"
                      disabled
                    >
                      Cancelled
                    </button>
                  )}
                </div>
              ),
            },
          ]}
          totalRecords={recordsData.all_counts}
          recordsPerPage={pageSize}
          page={page}
          onPageChange={(p) => setPage(p)}
          recordsPerPageOptions={PAGE_SIZES}
          onRecordsPerPageChange={(newPageSize) => {
            setPageSize(newPageSize);
            setPage(1);
          }}
          sortStatus={sortStatus}
          onSortStatusChange={setSortStatus}
          minHeight={200}
          paginationText={({ from, to, totalRecords }) =>
            `Showing  ${from} to ${to} of ${totalRecords} entries`
          }
        />
      </div>
      <UnbookModal
        title="Cancel booking?"
        message="Are you sure you want to cancel this apartment booking?"
        isOpen={isUnbookModal}
        onClose={() => setIsUnbookModal(false)}
        onConfirm={cancelBooking}
      />
    </div>
  );
};

export default Bookings;

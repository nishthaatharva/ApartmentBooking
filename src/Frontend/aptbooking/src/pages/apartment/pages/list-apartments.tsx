import { dataTableProps } from "../../../utils/common/constants";
import { useEffect, useState } from "react";
import { DataTable, DataTableSortStatus } from "mantine-datatable";
import { ActionIcon, TextInput } from "@mantine/core";
import SearchIcon from "../../../components/Shared/Icons/search-icon";
import CloseIcon from "../../../components/Shared/Icons/close-icon";
import Tippy from "@tippyjs/react";
import "tippy.js/dist/tippy.css";
import DeleteApartmentModal from "../../../components/Shared/delete-modal";
import filterService from "../../../utils/filter.service";
import { PaginationFilter } from "../../../utils/types/pagination-filter.interface";
import axiosInstance from "../../../utils/api.service";
import { APIs } from "../../../utils/common/api-paths";
import { useDebouncedValue } from "@mantine/hooks";
import messageService from "../../../utils/message.service";
import ManageApartmentModal from "../components/manage-apartment";
import LocalStorageService from "../../../utils/localstorage.service";

const Apartments = () => {
  const PAGE_SIZES = dataTableProps.PAGE_SIZES;
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(PAGE_SIZES[0]);
  const [recordsData, setRecordsData] = useState({ data: [], all_counts: 0 });
  const [params, setParams] = useState(filterService.defaultFilter());
  const [sortStatus, setSortStatus] = useState<DataTableSortStatus>({
    columnAccessor: "name",
    direction: "asc",
  });
  const [isdeleteApartmentModal, setIsDeleteApartmentModal] =
    useState<any>(false);

  const [deletedApartmentId, setdeletedApartmentId] = useState<string>("");
  const [apartmentFilter, setApartmentFilter] = useState("");
  const [locationFilter, setLocationFilter] = useState("");
  const [debouncedApartmentFilter] = useDebouncedValue(apartmentFilter, 500);
  const [debouncedLocationFilter] = useDebouncedValue(locationFilter, 500);
  const [managedApartmentId, setManagedApartmentId] = useState<string>("");
  const [isManageApartmentModal, setIsManageApartmentModal] =
    useState<any>(false);

  const localStorageService = LocalStorageService.getService();
  const userInfo = localStorageService.getUser();

  useEffect(() => {
    const filterarray: any[] = [];
    if (debouncedApartmentFilter) {
      filterarray.push({
        Field: "name",
        Value: debouncedApartmentFilter,
        Operator: "contain",
      });
    }
    if (debouncedLocationFilter) {
      filterarray.push({
        Field: "location",
        Value: debouncedLocationFilter,
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
  }, [
    sortStatus,
    page,
    pageSize,
    debouncedApartmentFilter,
    debouncedLocationFilter,
  ]);

  useEffect(() => {
    bindApartments(params);
  }, [params]);

  const bindApartments = async (params: PaginationFilter) => {
    const response = await axiosInstance.post(APIs.searchApartmentApi, {
      PaginationFilter: params, // Include pagination filter directly
    });
    if (response.data) {
      setRecordsData({
        data: response.data.data,
        all_counts: response.data.totalCount,
      });
    }
  };

  const manageApartmentConfirm = (id: string) => {
    setManagedApartmentId(id);
    setTimeout(() => {
      setIsManageApartmentModal(true);
    }, 500);
  };

  const deleteApartmentConfirm = (id: string) => {
    setdeletedApartmentId(id);
    setIsDeleteApartmentModal(true);
  };

  const deleteApartment = async () => {
    const response = await axiosInstance.delete(
      APIs.deleteApartmentApi + deletedApartmentId
    );
    if (response.data) {
      messageService.showMessage(response.data.data);
      bindApartments(params);
      setIsDeleteApartmentModal(false);
    }
  };

  const onSaveManageApartment = () => {
    setManagedApartmentId("");
    bindApartments(params);
    setIsManageApartmentModal(false);
  };

  const onCloseManageApartment = () => {
    setManagedApartmentId("");
    setIsManageApartmentModal(false);
  };

  return (
    <div className="panel">
      <div className="flex md:items-center md:flex-row flex-col mb-5 gap-5">
        <h5 className="font-semibold text-lg dark:text-white-light">
          Apartments
        </h5>
        {/* <div className="ltr:ml-auto rtl:mr-auto">
          <button
            type="button"
            className="btn btn-outline-primary btn-sm"
            onClick={() => setIsManageApartmentModal(true)}
          >
            Add Apartment
          </button>
        </div> */}
        {userInfo!.roles === "Administrator" && (
          <div className="ltr:ml-auto rtl:mr-auto">
            <button
              type="button"
              className="btn btn-outline-primary btn-sm"
              onClick={() => setIsManageApartmentModal(true)}
            >
              Add Apartment
            </button>
          </div>
        )}
      </div>
      <div className="datatables">
        <DataTable
          highlightOnHover
          className="whitespace-nowrap table-hover"
          records={recordsData.data}
          columns={[
            {
              accessor: "name",
              title: "Apartment Name",
              filter: (
                <TextInput
                  label="Apartment Name"
                  description="Show apartment whose names include specified texts"
                  placeholder="Search apartment..."
                  leftSection={<SearchIcon />}
                  rightSection={
                    <ActionIcon
                      size="sm"
                      variant="transparent"
                      c="dimmed"
                      onClick={() => setApartmentFilter("")}
                    >
                      <CloseIcon size={24} />
                    </ActionIcon>
                  }
                  value={apartmentFilter}
                  onChange={(e) => setApartmentFilter(e.currentTarget.value)}
                />
              ),
              filtering: apartmentFilter !== "",
              sortable: true,
            },
            {
              accessor: "location",
              title: "Location",
              filter: (
                <TextInput
                  label="Location"
                  description="Show apartment whose location include the specified text"
                  placeholder="Search location..."
                  leftSection={<SearchIcon />}
                  rightSection={
                    <ActionIcon
                      size="sm"
                      variant="transparent"
                      c="dimmed"
                      onClick={() => setLocationFilter("")}
                    >
                      <CloseIcon size={24} />
                    </ActionIcon>
                  }
                  value={locationFilter}
                  onChange={(e) => setLocationFilter(e.currentTarget.value)}
                />
              ),
              filtering: locationFilter !== "",
              sortable: true,
            },

            { accessor: "rooms", title: "BHK", sortable: true },
            {
              accessor: "statusName",
              title: "Status",
              // filter: (
              //   <TextInput
              //     label="Status"
              //     description="Show apartment whose status include the specified text"
              //     placeholder="Search status..."
              //     leftSection={<SearchIcon />}
              //     rightSection={
              //       <ActionIcon
              //         size="sm"
              //         variant="transparent"
              //         c="dimmed"
              //         onClick={() => setEmailFilter("")}
              //       >
              //         <CloseIcon size={24} />
              //       </ActionIcon>
              //     }
              //     value={emailFilter}
              //     onChange={(e) => setEmailFilter(e.currentTarget.value)}
              //   />
              // ),
              // filtering: emailFilter !== "",
              sortable: true,
            },
            {
              accessor: "action",
              title: "Action",
              titleClassName: "!text-center",
              render: ({ id }) => (
                <div className="flex items-center w-max mx-auto gap-2">
                  {userInfo?.roles === "Administrator" && (
                    <Tippy content="Edit">
                      <button
                        type="button"
                        onClick={() => manageApartmentConfirm(`${id}`)}
                      >
                        <svg
                          width="24"
                          height="24"
                          viewBox="0 0 24 24"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                          className="w-5 h-5 text-success"
                        >
                          <path
                            d="M15.2869 3.15178L14.3601 4.07866L5.83882 12.5999L5.83881 12.5999C5.26166 13.1771 4.97308 13.4656 4.7249 13.7838C4.43213 14.1592 4.18114 14.5653 3.97634 14.995C3.80273 15.3593 3.67368 15.7465 3.41556 16.5208L2.32181 19.8021L2.05445 20.6042C1.92743 20.9852 2.0266 21.4053 2.31063 21.6894C2.59466 21.9734 3.01478 22.0726 3.39584 21.9456L4.19792 21.6782L7.47918 20.5844L7.47919 20.5844C8.25353 20.3263 8.6407 20.1973 9.00498 20.0237C9.43469 19.8189 9.84082 19.5679 10.2162 19.2751C10.5344 19.0269 10.8229 18.7383 11.4001 18.1612L11.4001 18.1612L19.9213 9.63993L20.8482 8.71306C22.3839 7.17735 22.3839 4.68748 20.8482 3.15178C19.3125 1.61607 16.8226 1.61607 15.2869 3.15178Z"
                            stroke="currentColor"
                            strokeWidth="1.5"
                          />
                          <path
                            opacity="0.5"
                            d="M14.36 4.07812C14.36 4.07812 14.4759 6.04774 16.2138 7.78564C17.9517 9.52354 19.9213 9.6394 19.9213 9.6394M4.19789 21.6777L2.32178 19.8015"
                            stroke="currentColor"
                            strokeWidth="1.5"
                          />
                        </svg>
                      </button>
                    </Tippy>
                  )}
                  {userInfo?.roles === "Administrator" && (
                    <Tippy content="Delete">
                      <button
                        type="button"
                        onClick={() => deleteApartmentConfirm(`${id}`)}
                      >
                        <svg
                          className="text-danger"
                          width="20"
                          height="20"
                          viewBox="0 0 24 24"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            opacity="0.5"
                            d="M9.17065 4C9.58249 2.83481 10.6937 2 11.9999 2C13.3062 2 14.4174 2.83481 14.8292 4"
                            stroke="currentColor"
                            strokeWidth="1.5"
                            strokeLinecap="round"
                          />
                          <path
                            d="M20.5001 6H3.5"
                            stroke="currentColor"
                            strokeWidth="1.5"
                            strokeLinecap="round"
                          />
                          <path
                            d="M18.8334 8.5L18.3735 15.3991C18.1965 18.054 18.108 19.3815 17.243 20.1907C16.378 21 15.0476 21 12.3868 21H11.6134C8.9526 21 7.6222 21 6.75719 20.1907C5.89218 19.3815 5.80368 18.054 5.62669 15.3991L5.16675 8.5"
                            stroke="currentColor"
                            strokeWidth="1.5"
                            strokeLinecap="round"
                          />
                          <path
                            opacity="0.5"
                            d="M9.5 11L10 16"
                            stroke="currentColor"
                            strokeWidth="1.5"
                            strokeLinecap="round"
                          />
                          <path
                            opacity="0.5"
                            d="M14.5 11L14 16"
                            stroke="currentColor"
                            strokeWidth="1.5"
                            strokeLinecap="round"
                          />
                        </svg>
                      </button>
                    </Tippy>
                  )}
                  {userInfo?.roles === "User" && (
                    <button
                      type="button"
                      className="btn btn-outline-primary btn-sm"
                    >
                      Book
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
      <DeleteApartmentModal
        title="Delete Apartment"
        message="Are you sure you want to delete this apartment?"
        isOpen={isdeleteApartmentModal}
        onClose={() => setIsDeleteApartmentModal(false)}
        onDelete={deleteApartment}
      />
      <ManageApartmentModal
        manageApartmentId={managedApartmentId}
        isOpen={isManageApartmentModal}
        onClose={onCloseManageApartment}
        onSave={onSaveManageApartment}
      />
    </div>
  );
};

export default Apartments;

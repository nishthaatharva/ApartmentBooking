import { Fragment, useEffect, useState } from "react";
import { Transition, Dialog } from "@headlessui/react";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import messageService from "../../../utils/message.service";
import { APIs } from "../../../utils/common/api-paths";
import axiosInstance from "../../../utils/api.service";
import { FieldValidation, Regex } from "../../../utils/common/constants";

interface ManageApartmentModalProps {
  manageApartmentId: string;
  isOpen: boolean;
  onClose: () => void;
  onSave: () => void;
}

const ManageApartmentModal: React.FC<ManageApartmentModalProps> = ({
  manageApartmentId,
  isOpen,
  onClose,
  onSave,
}) => {
  const [apartmentDetails, setApartmentDetails] = useState<{
    name: string;
    location: string;
    size: number;
    rooms: number;
    status: number;
    apartmentAmenitiesAssociation: string[];
  }>({
    name: "",
    location: "",
    size: 0,
    rooms: 0,
    status: 1,
    apartmentAmenitiesAssociation: [],
  });
  const [originalObj, setOriginalObj] = useState<string>("");

  useEffect(() => {
    const fetchApartmentDetails = async () => {
      const response = await axiosInstance.get(
        APIs.getApartmentApi + manageApartmentId
      );
      if (response.data) {
        setApartmentDetails({
          name: response.data.data.name,
          location: response.data.data.location,
          size: response.data.data.size,
          rooms: response.data.data.rooms,
          status: response.data.data.status,
          apartmentAmenitiesAssociation:
            response.data.data.apartmentAmenitiesAssociation,
        });
        setOriginalObj(
          JSON.stringify({
            name: response.data.data.name,
            location: response.data.data.location,
            size: response.data.data.size,
            rooms: response.data.data.rooms,
            status: response.data.data.status,
            apartmentAmenitiesAssociation:
              response.data.data.apartmentAmenitiesAssociation,
          })
        );
      }
    };

    if (manageApartmentId) {
      fetchApartmentDetails();
    }
  }, [manageApartmentId]);

  const resetForm = () => {
    setApartmentDetails({
      name: "",
      location: "",
      size: 0,
      rooms: 0,
      status: 1,
      apartmentAmenitiesAssociation: [],
    });
  };

  const resetAndClose = () => {
    if (manageApartmentId) resetForm();
    onClose();
  };

  const submitForm = async (values: any) => {
    const response = manageApartmentId
      ? await axiosInstance.put(APIs.updateApartment + manageApartmentId, {
          ...values,
          id: manageApartmentId,
        })
      : await axiosInstance.post(APIs.addApartment, values);

    if (response.data) {
      messageService.showMessage(response.data.data);
      resetForm();
      onSave();
    }
  };

  const SubmittedForm = Yup.object().shape({
   
  });

  return (
    <Transition appear show={isOpen} as={Fragment}>
      <Dialog
        as="div"
        open={isOpen}
        onClose={() => (isOpen = true)}
        className="relative z-50"
      >
        <Transition.Child
          as={Fragment}
          enter="ease-out duration-300"
          enterFrom="opacity-0"
          enterTo="opacity-100"
          leave="ease-in duration-200"
          leaveFrom="opacity-100"
          leaveTo="opacity-0"
        >
          <div className="fixed inset-0 bg-[black]/60" />
        </Transition.Child>
        <div className="fixed inset-0 z-[999] px-4 overflow-y-auto">
          <div className="flex items-center justify-center min-h-screen">
            <Transition.Child
              as={Fragment}
              enter="ease-out duration-300"
              enterFrom="opacity-0 scale-95"
              enterTo="opacity-100 scale-100"
              leave="ease-in duration-200"
              leaveFrom="opacity-100 scale-100"
              leaveTo="opacity-0 scale-95"
            >
              <Dialog.Panel className="panel border-0 p-0 rounded-lg overflow-hidden w-full max-w-lg text-black dark:text-white-dark">
                <button
                  type="button"
                  onClick={resetAndClose}
                  className="absolute top-4 ltr:right-4 rtl:left-4 text-gray-400 hover:text-gray-800 dark:hover:text-gray-600 outline-none"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="20"
                    height="20"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  >
                    <line x1="18" y1="6" x2="6" y2="18"></line>
                    <line x1="6" y1="6" x2="18" y2="18"></line>
                  </svg>
                </button>
                <div className="text-lg font-medium bg-[#fbfbfb] dark:bg-[#121c2c] ltr:pl-5 rtl:pr-5 py-3 ltr:pr-[50px] rtl:pl-[50px]">
                  {manageApartmentId ? "Edit Apartment" : "Add Apartment"}
                </div>
                <div className="p-5">
                  <Formik
                    initialValues={{
                      name: apartmentDetails.name,
                      location: apartmentDetails.location,
                      size: apartmentDetails.size,
                      rooms: apartmentDetails.rooms,
                      status: apartmentDetails.status,
                      apartmentAmenitiesAssociation:
                        apartmentDetails.apartmentAmenitiesAssociation,
                    }}
                    validationSchema={SubmittedForm}
                    onSubmit={() => {}}
                  >
                    {({ errors, submitCount, touched, values }) => (
                      <Form className="space-y-5">
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                          <div
                            className={
                              submitCount
                                ? errors.name
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="name">Apartment Name</label>
                            <Field
                              name="name"
                              type="text"
                              id="name"
                              placeholder="Enter Apartment Name"
                              className="form-input"
                            />
                            {submitCount ? (
                              errors.name ? (
                                <div className="text-danger mt-1">
                                  {errors.name}
                                </div>
                              ) : (
                                ""
                              )
                            ) : (
                              ""
                            )}
                          </div>
                          <div
                            className={
                              submitCount
                                ? errors.location
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="location">Location</label>
                            <Field
                              name="location"
                              type="text"
                              id="location"
                              placeholder="Enter Apartment Location"
                              className="form-input"
                            />
                            {submitCount ? (
                              errors.location ? (
                                <div className="text-danger mt-1">
                                  {errors.location}
                                </div>
                              ) : (
                                ""
                              )
                            ) : (
                              ""
                            )}
                          </div>
                        </div>
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                          <div
                            className={
                              submitCount
                                ? errors.size
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="size">Apartment Size(sq ft.)</label>
                            <Field
                              name="size"
                              type="number"
                              id="size"
                              placeholder="Enter Apartment Size"
                              className="form-input"
                            />
                            {submitCount ? (
                              errors.size ? (
                                <div className="text-danger mt-1">
                                  {errors.size}
                                </div>
                              ) : (
                                ""
                              )
                            ) : (
                              ""
                            )}
                          </div>
                          <div
                            className={
                              submitCount
                                ? errors.rooms
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="size">No. Of Rooms</label>
                            <Field
                              name="rooms"
                              type="number"
                              id="rooms"
                              placeholder="Enter Number Of Rooms"
                              className="form-input"
                            />
                            {submitCount ? (
                              errors.rooms ? (
                                <div className="text-danger mt-1">
                                  {errors.rooms}
                                </div>
                              ) : (
                                ""
                              )
                            ) : (
                              ""
                            )}
                          </div>
                        </div>
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                          <div
                            className={
                              submitCount
                                ? errors.status
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="status">Status</label>
                            {/* <Field
                              name="status"
                              as="select"
                              id="status"
                              className="form-select"
                              value={apartmentDetails.status} // Set the selected value here
                              onChange={(
                                event: React.ChangeEvent<HTMLSelectElement>
                              ) => {
                                setApartmentDetails((prevDetails) => ({
                                  ...prevDetails,
                                  status: parseInt(event.target.value), // Ensure value is parsed as integer
                                }));
                              }}
                            >
                              <option value={1}>Available</option>
                              <option value={2}>Reserved</option>
                            </Field> */}
                            <Field
                              name="status"
                              as="select"
                              id="status"
                              className="form-select"
                            >
                              <option
                                value={1}
                                selected={apartmentDetails.status === 1}
                              >
                                Available
                              </option>
                              <option
                                value={2}
                                selected={apartmentDetails.status === 2}
                              >
                                Reserved
                              </option>
                            </Field>
                            {submitCount ? (
                              errors.status ? (
                                <div className="text-danger mt-1">
                                  {errors.status}
                                </div>
                              ) : (
                                ""
                              )
                            ) : (
                              ""
                            )}
                          </div>
                        </div>
                        <div className="flex justify-end items-center mt-8">
                          <button
                            type="button"
                            className="btn btn-outline-danger"
                            onClick={resetAndClose}
                          >
                            Cancel
                          </button>
                          <button
                            type="submit"
                            className="btn btn-primary ltr:ml-4 rtl:mr-4"
                            // disabled={
                            //   manageApartmentId !== "" ? originalObj === JSON.stringify(values) : false
                            // }
                            onClick={() => {
                              if (
                                (manageApartmentId ||
                                  Object.keys(touched).length !== 0) &&
                                Object.keys(errors).length === 0
                              ) {
                                submitForm(values);
                              }
                            }}
                          >
                            {manageApartmentId ? "Update" : "Create"}
                          </button>
                        </div>
                      </Form>
                    )}
                  </Formik>
                </div>
              </Dialog.Panel>
            </Transition.Child>
          </div>
        </div>
      </Dialog>
    </Transition>
  );
};

export default ManageApartmentModal;

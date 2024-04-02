import { Fragment, useState } from "react";
import { Transition, Dialog } from "@headlessui/react";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import messageService from "../../../utils/message.service";
import { APIs } from "../../../utils/common/api-paths";
import axiosInstance from "../../../utils/api.service";
import DatePicker from "../../../components/Shared/DatePicker";
import commonService from "../../../utils/common.service";
import { NotificationType } from "../../../utils/common/constants";
//import { FieldValidation } from "../../../utils/common/constants";

interface ManageBookingModalProps {
  manageApartmentId: string;
  isOpen: boolean;
  onClose: () => void;
  onSave: () => void;
}

const ManageBookingModal: React.FC<ManageBookingModalProps> = ({
  manageApartmentId,
  isOpen,
  onClose,
  onSave,
}) => {
  const [bookingDetails, setbookingDetails] = useState<{
    bookFrom: Date;
    bookTill: Date;
    isOnLease: boolean;
    leaseDuration: number;
  }>({
    bookFrom: new Date(),
    bookTill: new Date(),
    isOnLease: false,
    leaseDuration: 0,
  });

  const resetAndClose = () => {
    if (manageApartmentId) resetForm();
    onClose();
  };

  const resetForm = () => {
    setbookingDetails({
      bookFrom: new Date(),
      bookTill: new Date(),
      isOnLease: false,
      leaseDuration: 0,
    });
  };

  const submitForm = async (values: any) => {
    const bookFrom = commonService.parseDate(values.bookFrom);
    const bookTill = commonService.parseDate(values.bookTill);

    const response = await axiosInstance.post(APIs.bookApartmentApi, {
      bookFrom: bookFrom.newDate,
      bookTill: bookTill.newDate,
      apartmentId: manageApartmentId,
      isOnLease: values.isOnLease,
      leaseDuration: values.leaseDuration,
    });

    if (response.data) {
      if (response.data.success === false) {
        messageService.showMessage(
          response.data.data,
          NotificationType.success
        );
        resetForm();
        onSave();
      } else {
        messageService.showMessage(
          response.data.data,
          NotificationType.success
        );
        resetForm();
        onSave();
      }
    }
  };

  const SubmittedForm = Yup.object().shape({});
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
                  {manageApartmentId ? "Book Apartment" : "Add Apartment"}
                </div>
                <div className="p-5">
                  <Formik
                    initialValues={{
                      bookFrom: bookingDetails.bookFrom,
                      bookTill: bookingDetails.bookTill,
                      isOnLease: bookingDetails.isOnLease,
                      leaseDuration: bookingDetails.leaseDuration,
                    }}
                    validationSchema={SubmittedForm}
                    onSubmit={() => {}}
                  >
                    {({
                      errors,
                      submitCount,
                      touched,
                      values,
                      setFieldValue,
                    }) => (
                      <Form className="space-y-5">
                        {/* <div className="flex justify-end items-center mt-8"> */}
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                          <div
                            className={
                              submitCount
                                ? errors.bookFrom
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="bookFrom">Book From</label>
                            <Field
                              as={DatePicker}
                              name="bookFrom"
                              placeholder="Enter date to book from"
                              onChange={(event: any) => {
                                setFieldValue("bookFrom", event);
                              }}
                            />

                            {submitCount ? (
                              errors.bookFrom == null ? (
                                <div className="text-danger mt-1">
                                  {errors.bookFrom}
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
                                ? errors.bookTill
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            {values.isOnLease && (
                              <label className="text-primary">
                                'Booking Till' date will be automatically
                                selected from lease duration.
                              </label>
                            )}
                            {!values.isOnLease && (
                              <label htmlFor="bookTill">Book Till</label>
                            )}
                            {!values.isOnLease && (
                              <Field
                                as={DatePicker}
                                name="bookTill"
                                placeholder="Enter date to book till"
                                onChange={(event: any) => {
                                  setFieldValue("bookTill", event);
                                }}
                                disabled={values.isOnLease}
                              />
                            )}
                            {submitCount ? (
                              errors.bookTill == null ? (
                                <div className="text-danger mt-1">
                                  {errors.bookTill}
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
                                ? errors.isOnLease
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="isOnLease">Book On Lease?</label>
                            <Field
                              type="checkbox"
                              name="isOnLease"
                              checked={values.isOnLease}
                              onChange={(
                                e: React.ChangeEvent<HTMLInputElement>
                              ) => setFieldValue("isOnLease", e.target.checked)}
                            />
                            {submitCount ? (
                              errors.isOnLease ? (
                                <div className="text-danger mt-1">
                                  {errors.isOnLease}
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
                                ? errors.leaseDuration
                                  ? "has-error"
                                  : ""
                                : ""
                            }
                          >
                            <label htmlFor="leaseDuration">
                              Lease Duration
                            </label>
                            <Field
                              name="leaseDuration"
                              as="select"
                              id="leaseDuration"
                              className="form-select"
                              disabled={!values.isOnLease}
                            >
                              <option
                                value={0}
                                disabled={true}
                                selected={!values.leaseDuration}
                              >
                                Select lease duration
                              </option>
                              <option
                                value={1}
                                selected={bookingDetails.leaseDuration === 1}
                              >
                                Week
                              </option>
                              <option
                                value={2}
                                selected={bookingDetails.leaseDuration === 2}
                              >
                                Month
                              </option>
                              <option
                                value={3}
                                selected={bookingDetails.leaseDuration === 3}
                              >
                                Year
                              </option>
                            </Field>
                            {submitCount ? (
                              errors.leaseDuration ? (
                                <div className="text-danger mt-1">
                                  {errors.leaseDuration}
                                </div>
                              ) : (
                                ""
                              )
                            ) : (
                              ""
                            )}
                          </div>
                        </div>
                        <div className="flex justify-start items-center mt-8">
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
                            Book
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

export default ManageBookingModal;

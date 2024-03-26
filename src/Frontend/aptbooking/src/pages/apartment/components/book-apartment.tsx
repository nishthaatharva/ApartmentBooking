interface ManageBookingModalProps {
  manageApartmentId: string;
  isOpen: boolean;
  onClose: () => void;
  onSave: () => void;
}

const ManageBookingModal: React.FC<ManageBookingModalProps> = () => {
  return (
    <>
      <p>Book</p>
    </>
  );
};

export default ManageBookingModal;
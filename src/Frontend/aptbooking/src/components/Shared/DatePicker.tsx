import { CSSProperties, FC } from "react";
import Flatpickr from "react-flatpickr";
import "flatpickr/dist/flatpickr.css";
import { useSelector } from "react-redux";
import { IRootState } from "../../store";

interface DatePickerProps {
  id: string;
  name: string;
  placeholder?: string;
  options?: Record<string, any>;
  value: string | Date | undefined;
  className?: string;
  style?: CSSProperties;
  onChange: (
    selectedDates: Date[] | Date | undefined,
    dateStr: string,
    instance: any
  ) => void;
  disabled?: boolean; // Add disabled prop
}

const DatePicker: FC<DatePickerProps> = ({
  id,
  name,
  placeholder,
  options = {},
  value,
  className,
  style,
  onChange,
  disabled = false,
}) => {
  const isRtl =
    useSelector((state: IRootState) => state.themeConfig.rtlClass) === "rtl"
      ? true
      : false;

  return (
    <Flatpickr
      id={id}
      name={name}
      placeholder={placeholder}
      options={{
        dateFormat: "Y-m-d",
        position: isRtl ? "auto right" : "auto left",
        ...options,
      }}
      value={value}
      className={"form-input " + className}
      style={style}
      onChange={onChange}
      disabled={disabled}
    />
  );
};

export default DatePicker;

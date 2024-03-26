import { useNavigate } from "react-router-dom";
import { Field, Form, Formik } from "formik";
import * as Yup from "yup";
import authService from "../utils/auth.service";
import { Regex } from "../../../utils/common/constants";

const SignIn = () => {
  const navigate = useNavigate();

  const submitForm = (values: any) => {
    const { email, password } = values;

    const promise = authService.login(email, password);

    promise.then(() => {
      navigate("/");
    });
  };

  const SubmittedForm = Yup.object().shape({
    email: Yup.string()
      .required("Email can not be empty")
      .email("Invalid email format")
      .matches(Regex.emailValidationPattern, "Invalid email format"),
    password: Yup.string().required("Password can not be empty"),
  });

  return (
    <div className="flex justify-center items-center min-h-screen bg-cover bg-center bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')]">
      <div className="panel sm:w-[480px] m-6 max-w-lg w-full">
        <h2 className="font-bold text-2xl mb-3">Sign In</h2>
        <p className="mb-7">Enter your email and password to sign in</p>
        <Formik
          initialValues={{ email: "", password: "" }}
          validationSchema={SubmittedForm}
          onSubmit={() => {}}
        >
          {({ errors, submitCount, isValid, values }) => (
            <Form className="space-y-5">
              <div
                className={submitCount ? (errors.email ? "has-error" : "") : ""}
              >
                <label htmlFor="email">Email</label>
                <Field
                  name="email"
                  id="email"
                  type="email"
                  className="form-input"
                  placeholder="Enter Email"
                />
                {submitCount ? (
                  errors.email ? (
                    <div className="text-danger mt-1">{errors.email}</div>
                  ) : (
                    " "
                  )
                ) : (
                  ""
                )}
              </div>
              <div
                className={
                  submitCount ? (errors.password ? "has-error" : "") : ""
                }
              >
                <label htmlFor="password">Password</label>
                <Field
                  name="password"
                  id="password"
                  type="password"
                  className="form-input"
                  placeholder="Enter Password"
                />
                {submitCount ? (
                  errors.password ? (
                    <div className="text-danger mt-1">{errors.password}</div>
                  ) : (
                    ""
                  )
                ) : (
                  ""
                )}
              </div>
              <button
                type="submit"
                className="btn btn-primary w-full"
                onClick={() => {
                  if (isValid && values.email && values.password) {
                    submitForm(values);
                  }
                }}
              >
                SIGN IN
              </button>
            </Form>
          )}
        </Formik>
      </div>
    </div>
  );
};

export default SignIn;

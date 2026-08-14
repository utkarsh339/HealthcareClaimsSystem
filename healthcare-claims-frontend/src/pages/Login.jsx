import { getErrorMessage } from "../utils_errorMessage";
import { useState } from "react";
import { useDispatch } from "react-redux";
import { Link, useNavigate } from "react-router-dom";

import { useLoginMutation } from "../features/auth/authApi";
import { setCredentials } from "../features/auth/authSlice";

function Login() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [login, { isLoading }] = useLoginMutation();

  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const [error, setError] = useState("");

  const handleChange = (event) => {
    const { name, value } = event.target;

    setFormData((previous) => ({
      ...previous,
      [name]: value,
    }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");

    try {
      const response = await login(formData).unwrap();

      dispatch(
        setCredentials({
          token: response.token,
          user: {
            userId: response.userId,
            fullName: response.fullName,
            email: response.email,
            role: response.role,
          },
        }),
      );

      navigate("/dashboard");
    } catch (error) {
      console.error(error);

      setError(
        getErrorMessage(error, "Login failed. Please check your credentials."),
      );
    }
  };

  return (
    <div>
      <h1>Healthcare Claims System</h1>

      <h2>Login</h2>

      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="email">Email</label>

          <input
            id="email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </div>

        <div>
          <label htmlFor="password">Password</label>

          <input
            id="password"
            name="password"
            type="password"
            value={formData.password}
            onChange={handleChange}
            required
          />
        </div>

        {error && <p className="auth-error" role="alert">{error}</p>}

        <button type="submit" disabled={isLoading}>
          {isLoading ? "Logging in..." : "Login"}
        </button>
      </form>

      <p>
        Don&apos;t have an account? <Link to="/register">Create an account</Link>
      </p>
    </div>
  );
}

export default Login;

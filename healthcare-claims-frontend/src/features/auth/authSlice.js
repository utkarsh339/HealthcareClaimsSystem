import { createSlice } from "@reduxjs/toolkit";

const storedToken = localStorage.getItem("accessToken");
const storedUser = localStorage.getItem("user");

const initialState = {
  token: storedToken || null,
  user: storedUser ? JSON.parse(storedUser) : null,
};

const authSlice = createSlice({
  name: "auth",

  initialState,

  reducers: {
    setCredentials: (state, action) => {
      const { token, user } = action.payload;

      state.token = token;
      state.user = user;

      localStorage.setItem("accessToken", token);
      localStorage.setItem("user", JSON.stringify(user));
    },

    logout: (state) => {
      state.token = null;
      state.user = null;

      localStorage.removeItem("accessToken");
      localStorage.removeItem("user");
    },
  },
});

export const { setCredentials, logout } = authSlice.actions;

export default authSlice.reducer;

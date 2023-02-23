import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import auth from "../components/UserAuthentication/CognitoAuthentication";
import { CognitoAuthSession } from "amazon-cognito-auth-js";


interface CognitoError {
  code: string;
  message: string;
}


export const authenticateUser = createAsyncThunk(
  'auth/authenticateuser',
  async() => {
    try {
      // sign in the user using the AWS hosted UI
      await new Promise((resolve, reject) => {
        auth.userhandler = {
          onSuccess: (result: CognitoAuthSession) => {
            resolve(result);
          },
          onFailure: (err: CognitoError) => {
            reject(err);
          },
        };
        auth.getSession();
      });
      
      // get the access token and refresh token from the session
      const signInUserSession = auth.getSignInUserSession();
      if (!signInUserSession) {
        throw new Error('Unable to retrieve signInUserSession');
      }

      const accessToken = signInUserSession.getAccessToken().getJwtToken();
      const refreshToken = signInUserSession.getRefreshToken().getToken();
    
    // return the access token and refresh token
    return { accessToken, refreshToken };
    
  } catch (error) {
    return error
  }  
});

// slice to authenticate user login
const authSlice = createSlice({
  name: "auth",
  initialState: {
    isAuthenticated: false,
    accessToken: '',
    refreshToken: '',
    error: ''
  },
  reducers: {
    loginSuccess: (state, action) => {
      state.isAuthenticated = true;
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
    },
    loginFailure: (state, action) => {
      state.isAuthenticated = false;
      state.error = action.payload;
    },
  },
});

export const { loginSuccess, loginFailure } = authSlice.actions;
export default authSlice.reducer;
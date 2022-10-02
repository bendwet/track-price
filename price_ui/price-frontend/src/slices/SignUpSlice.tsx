import React from "react";
import UserPool from "../UserPool";
import { createAsyncThunk ,createSlice, PayloadAction } from "@reduxjs/toolkit";
import User from "../components/Models/User";

export const signUp = createAsyncThunk(
    'user/register',
    async(userDetails: User) => {
        UserPool.signUp(userDetails.email, userDetails.password, [], [], (error, data) => {
            if (error) {
                console.error(error)
            }
            console.log(data)
        })
    }
);


export const signUpSlice = createSlice({
    name: 'signup',
    initialState: {
        isLoggedIn: false,
        email: '',
        password: ''
    },
    reducers : {
        setEmail(state, userInput: PayloadAction<string>) {
            state.email = userInput.payload;
        },
        setPassword(state, userInput: PayloadAction<string>) {
            state.password = userInput.payload;
        }

    },

    extraReducers: (builder) => {
        builder
        .addCase(signUp.fulfilled, (state) => {
            state.isLoggedIn = true;
        })
    }

});

export const { setEmail, setPassword } = signUpSlice.actions
export default signUpSlice.reducer;

import { createSlice } from '@reduxjs/toolkit'
import axios from 'axios';

export const productApiSlice = createSlice({
    name: 'counter',
    initialState: {
        value: '',
    },
    reducers: {
        retrieveProduct: (state) => {
            // retrieve product name, size and store name
            axios({
                method: 'get',
                url: 'http://127.0.0.1:5000/retrieve_product',
              })
               // Log returned result
            .then((res) => {
                console.log(res.data);
            })
            // Otherwise log error
            .catch((err) => {
                console.log(err);
            });
            console.log('retrieved product info');
        },
    }
});

// Action creators are generated for each case reducer function
export const { retrieveProduct } = productApiSlice.actions

export default productApiSlice.reducer

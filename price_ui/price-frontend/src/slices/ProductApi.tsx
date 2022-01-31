import { createSlice } from '@reduxjs/toolkit'


export const productApiSlice = createSlice({
    name: 'counter',
    initialState: {
        value: '',
    },
    reducers: {
        retrieveProduct: (state) => {
            // retrieve product name, size and store name
            console.log('retrieved store info');
        },
    }
});

// Action creators are generated for each case reducer function
export const { retrieveProduct } = productApiSlice.actions

export default productApiSlice.reducer

import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'
import axios from 'axios';

export const getProduct: any = createAsyncThunk(
  'databaseProductApi/getProduct',
  async(thunkAPI) => {
    const response = await fetch('http://127.0.0.1:5000/retrieve_product').then(
      (data) => data.json()
    );
    return response
  }
);
    

//       // retrieve product name, size and store name
//       const response = axios({
//         method: 'get',
//         url: 'http://127.0.0.1:5000/retrieve_product',
//     })
//     // Log returned result
//     .then((res) => {
//       console.log(res.data);
//       return res.data
//     })
//   // Otherwise log error
//     .catch((err) => {
//       console.log(err);
//       return err
//     });
  
//     console.log('retrieved product info');
//     return response


export const productApiSlice = createSlice({
	name: 'productApi',
	initialState: {
    products: '',
		status: ''
  },
  reducers: {},
  // extra reducers handle async requests
  extraReducers: {
    [getProduct.fulfilled]: (state, action) => {
			state.status = 'success';
      state.products = action.payload;
    },

		[getProduct.pending]: (state) => {
			state.products = 'loading';
		},

		[getProduct.rejected]: (state) => {
			state.status = 'failed';
		}

  },
});

export default productApiSlice.reducer

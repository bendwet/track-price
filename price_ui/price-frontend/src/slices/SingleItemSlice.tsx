import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import ProductModel from '../components/ProductModel';


// get products from database
export const getProductById = createAsyncThunk(
  'databaseProductApi/getProductById',
  async(productId: number) => {
    const response: Array<ProductModel> = await fetch(`http://127.0.0.1:5000/product/${productId}`)
		.then(
      (data) => data.json()
    );
    return response;
  }
);

export const singleItemSlice = createSlice({
	name: 'singleItem',
	initialState: {
		singleProduct: [] as Array<ProductModel>
	},
	reducers: {},
	extraReducers: (builder) => {
		builder
		.addCase(getProductById.fulfilled, (state, action) => {
			state.singleProduct = action.payload;
		})
	}

});

export default singleItemSlice.reducer;
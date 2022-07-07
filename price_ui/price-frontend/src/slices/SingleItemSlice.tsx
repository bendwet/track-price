import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import ProductModel from '../components/ProductModel';


// get products from database
export const getProductById = createAsyncThunk(
  'databaseProductApi/getProductById',
  async(productId: string) => {
    const response: Array<ProductModel> = await fetch(`https://localhost:7135/items/${productId}`)
		.then(
      (data) => data.json()
    );
    return response;
  }
);


export const singleItemSlice = createSlice({
	name: 'singleProduct',
	initialState: {
		singleProduct: [] as Array<ProductModel>,
		selectedProduct: {} as ProductModel
	},
	reducers: {	
		reset(state) {
			state.singleProduct = [] as Array<ProductModel>;
		}
	},
	extraReducers: (builder) => {
		builder
		.addCase(getProductById.fulfilled, (state, action) => {
			state.singleProduct = action.payload;
			// set selectedProduct = the first object of retruned ProductModel array
			state.selectedProduct = action.payload[0]
		})
	}

});

export const { reset } = singleItemSlice.actions;
export default singleItemSlice.reducer;
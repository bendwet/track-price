import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import ProductModel from '../components/ProductModel';


// get price from database
export const getPriceById = createAsyncThunk(
  'databaseProductApi/getPriceById',
  async(productId: string) => {
    const response: Array<ProductModel> = await fetch(`https://localhost:7135/items/${productId}/lowest-price`)
		.then(
      (data) => data.json()
    );
    return response;
  }
);


export const singleItemChartSlice = createSlice({
	name: 'lowestPriceHistoryChart',
	initialState: {
		lowestPriceHistory: [] as Array<ProductModel>
	},
	reducers: {	
		reset(state) {
			state.lowestPriceHistory = [] as Array<ProductModel>;
		}
	},
	extraReducers: (builder) => {
		builder
		.addCase(getPriceById.fulfilled, (state, action) => {
			state.lowestPriceHistory = action.payload;
		})
	}

});

export const { reset } = singleItemChartSlice.actions;
export default singleItemChartSlice.reducer;
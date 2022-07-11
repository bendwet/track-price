import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import Item from '../components/Models/Item';


// get price from database
export const getPriceById = createAsyncThunk(
  'databaseProductApi/getPriceById',
  async(productId: string) => {
    const response: Array<Item> = await fetch(`https://localhost:7135/items/${productId}/lowest-price`)
		.then(
      (data) => data.json()
    );
    return response;
  }
);


export const singleItemChartSlice = createSlice({
	name: 'lowestPriceHistoryChart',
	initialState: {
		lowestPriceHistory: [] as Array<Item>
	},
	reducers: {	
		reset(state) {
			state.lowestPriceHistory = [] as Array<Item>;
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
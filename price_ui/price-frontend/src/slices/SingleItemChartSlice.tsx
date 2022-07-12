import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import LowestPriceHistoryItem from '../components/Models/LowestPriceHistoryItem';


// get price from database
export const getLowestPriceItemPerDate = createAsyncThunk(
  'databaseProductApi/getLowestPriceHistory',
  async(productId: string) => {
    const response: Array<LowestPriceHistoryItem> = await fetch(`https://localhost:7135/items/${productId}/price-history`)
		.then(
      (data) => data.json()
    );
    return response;
  }
);


export const singleItemChartSlice = createSlice({
	name: 'lowestPriceHistoryChart',
	initialState: {
		lowestPriceHistory: [] as Array<LowestPriceHistoryItem>
	},
	reducers: {	
		reset(state) {
			state.lowestPriceHistory = [] as Array<LowestPriceHistoryItem>;
		}
	},
	extraReducers: (builder) => {
		builder
		.addCase(getLowestPriceItemPerDate.fulfilled, (state, action) => {
			state.lowestPriceHistory = action.payload;
		})
	}

});

export const { reset } = singleItemChartSlice.actions;
export default singleItemChartSlice.reducer;
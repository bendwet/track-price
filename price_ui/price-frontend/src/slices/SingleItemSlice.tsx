import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import SingleItem from '../components/Models/SingleItem';


// get products from database
export const getItemById = createAsyncThunk(
  'databaseProductApi/getItemtById',
  async(productId: string) => {
    const response: Array<SingleItem> = await fetch(`https://localhost:7135/items/${productId}`)
		.then(
      (data) => data.json()
    );
    return response;
  }
);


export const singleItemSlice = createSlice({
	name: 'singleItem',
	initialState: {
		singleItem: [] as Array<SingleItem>,
		selectedItem: {} as SingleItem
	},
	reducers: {	
		reset(state) {
			state.singleItem = [] as Array<SingleItem>;
		}
	},
	extraReducers: (builder) => {
		builder
		.addCase(getItemById.fulfilled, (state, action) => {
			state.singleItem = action.payload;
			// set selectedProduct = the first object of returned Item array
			state.selectedItem = action.payload[0]
		})
	}

});

export const { reset } = singleItemSlice.actions;
export default singleItemSlice.reducer;
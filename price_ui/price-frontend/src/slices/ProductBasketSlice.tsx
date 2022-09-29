import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import Item from '../components/Models/Item';


// get products from database
export const getItem = createAsyncThunk(
  'databaseProductApi/getItem',
  async() => {
    const response: Array<Item> = await fetch('https://localhost:7135/items').then(
      (data) => data.json()
    );
    return response;
  }
);

// filter products by search
export const filterItem = createAsyncThunk(
  'filterItems',
  async (values: Array<Array<Item>|string>) => {
   const products = values[0] as Array<Item>;
   const searchTerm = values[1] as string;
   let result: Array<Item> = products.filter((product: Item) => {
      // if searchTerm is blank, return products with no filter
      if (searchTerm === '') {
        return products;
      // else return products with filtered items 
      } else if (product.productName.toLowerCase().includes(searchTerm.toLowerCase())) {
        return products;
      }
    });
  return result;
  }
)

export const itemBasketSlice = createSlice({
	name: 'itemBasket',
	initialState: {
    items: [] as Array<Item>,
    filteredItems: [] as Array<Item>,
		status: ''
  },
  reducers: {
    reset(state) {
      state.items = [] as Array<Item>
      state.filteredItems = [] as Array<Item>
      state.status = ''
    }
  },
  // extra reducers handle async requests
  extraReducers: (builder) => {
    builder
    .addCase(getItem.fulfilled, (state, action) => {
			state.status = 'success';
      state.items = action.payload;
      state.filteredItems = action.payload;
    })

		.addCase(getItem.pending, (state) => {
			state.status = 'loading';
      state.items = [];
      state.filteredItems = [];
		})

		.addCase(getItem.rejected, (state) => {
			state.status = 'failed';
      state.items = [];
      state.filteredItems = [];
		})

    .addCase(filterItem.fulfilled, (state, action) => {
      state.filteredItems = action.payload;
    })
  }
});

export const { reset } = itemBasketSlice.actions;
export default itemBasketSlice.reducer;

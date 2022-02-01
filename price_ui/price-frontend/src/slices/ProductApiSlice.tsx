import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'

export const getProduct: any = createAsyncThunk(
  'databaseProductApi/getProduct',
  async(thunkAPI) => {
    const response = await fetch('http://127.0.0.1:5000/retrieve_product').then(
      (data) => data.json()
    );
    return response
  }
);

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

export default productApiSlice.reducer;

import { configureStore } from '@reduxjs/toolkit'
import productBasketSlice from '../slices/ProductBasketSlice'
import singleItemSlice from '../slices/SingleItemSlice'

export const store = configureStore({
  reducer: {
    products: productBasketSlice,
    singleItem: singleItemSlice,
  }
})

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch

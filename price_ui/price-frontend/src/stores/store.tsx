import { configureStore } from '@reduxjs/toolkit'
import productApiReducer from '../slices/ProductApiSlice'

export const store = configureStore({
  reducer: {
    products: productApiReducer
  }
})

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch

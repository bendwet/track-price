import { configureStore } from '@reduxjs/toolkit'
import itemBasketSlice from '../slices/ProductBasketSlice'
import singleItemSlice from '../slices/SingleItemSlice'
import singleItemChartSlice from '../slices/SingleItemChartSlice'

export const store = configureStore({
  reducer: {
    items: itemBasketSlice,
    singleItem: singleItemSlice,
    lowestPriceHistory: singleItemChartSlice
  }
})

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch

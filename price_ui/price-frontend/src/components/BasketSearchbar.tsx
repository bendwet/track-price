import React from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { RootState } from '../stores/store';
import { filterProducts } from '../slices/ProductBasketSlice';

function BasketSearchbar() {
  const dispatch = useDispatch();
	const { products } = useSelector((state: RootState) => state.products)
    
  return (
    <input type='text' placeholder='Search' onChange={event => dispatch(filterProducts([products, event.target.value]))}></input>
  );
}

export default BasketSearchbar;
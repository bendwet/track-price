import React from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { RootState } from '../../stores/store';
import { filterItem } from '../../slices/ProductBasketSlice';

function BasketSearchbar() {
  const dispatch = useDispatch();
	const { items } = useSelector((state: RootState) => state.items)
    
  return (
    <input type='text' placeholder='Search' onChange={event => dispatch(filterItem([items, event.target.value]))}></input>
  );
}

export default BasketSearchbar;
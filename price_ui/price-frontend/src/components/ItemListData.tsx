import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux'
import { RootState } from '../stores/store';
import { getProduct } from '../slices/ProductApiSlice';

function ItemListData() {
	const dispatch = useDispatch();
	const { products } = useSelector((state: RootState) => state.products) 

	useEffect(() => {
		dispatch(getProduct())
	}, [dispatch]);

	console.log(products)

  return (
    <div>
      <h1>test</h1>
			<h2>{ JSON.stringify(products[0]) }</h2>
    </div>
    );
}

export default ItemListData;

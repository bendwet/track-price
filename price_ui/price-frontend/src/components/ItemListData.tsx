import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux'
import { RootState, store } from '../stores/store';
import { getProduct } from '../slices/ProductApiSlice';
import { Dictionary } from '@reduxjs/toolkit';


function ItemListData() {
	const dispatch = useDispatch();
	const { products, status } = useSelector((state: RootState) => state.products) 
  
	useEffect(() => {
		dispatch(getProduct())
	}, [dispatch]);

  // define types for product elements
  interface ProductType {
    product_name?: string;
    unit_of_measure_size?: number;
    unit_of_measure?: string;
    price_sale?: string;
  }

  console.log(products)
  
  return (
    <div className='ItemListContainer'>
      <div className='RefreshButtonContainer'>
        <button className='RefreshButton' onClick={() => dispatch(getProduct())}>Refresh</button>
      </div>
      <div className='ItemListData'>
        <h1>{ JSON.stringify(status) }</h1>
        <ul className='ItemDisplay'>
        { products.map((product: ProductType, index: number) => 
            <li className='Items' key={index}>
                <span className='ProductName'>{product.product_name}</span>
                <span> {product.unit_of_measure_size}{product.unit_of_measure}</span>
                <span> ${product.price_sale}</span>
            </li>
          )}
        </ul>
      </div>
    </div>
    );
}

export default ItemListData;

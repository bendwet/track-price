import React, { useLayoutEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux'
import { RootState } from '../../stores/store';
import { getProduct } from '../../slices/ProductBasketSlice';
import Item from '../Models/Item';
import {Link} from 'react-router-dom';
import { reset } from '../../slices/ProductBasketSlice';

function ItemListData() {
	const dispatch = useDispatch();
	const { filteredProducts, status } = useSelector((state: RootState) => state.products) 
  
  // update state of products on page load
	useLayoutEffect(() => {
    dispatch(reset())
		dispatch(getProduct())
	}, [dispatch]);

  
  return (
    <div className='ItemListContainer'>
      <div className='RefreshButtonContainer'>
        <button className='RefreshButton' onClick={() => dispatch(getProduct())}>Refresh</button>
      </div>
      <div className='ItemListData'>
        <h1>{ JSON.stringify(status) }</h1>
        <ul className='ItemDisplay'>
        { filteredProducts.map((item: Item, index: number) => 
          <Link className='ItemLink' key={index} to={`/Item/${item.productId}`}>
            <li className='Items'>
                <span className='ProductName'>{item.productName}</span>
                <span> {item.priceQuantity}</span>
                <span> ${item.salePrice?.toFixed(2)}</span>
            </li>
          </Link>
          )}
        </ul>
      </div>
    </div>
  );
}

export default ItemListData;

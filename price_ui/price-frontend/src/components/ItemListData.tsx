import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux'
import { RootState } from '../stores/store';
import { getProduct } from '../slices/ProductBasketSlice';
import ProductModel from './ProductModel';
import {Link} from 'react-router-dom';


function ItemListData() {
	const dispatch = useDispatch();
	const { filteredProducts, status } = useSelector((state: RootState) => state.products) 
  
  // update state of products on page load
	useEffect(() => {
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
        { filteredProducts.map((product: ProductModel, index: number) => 
          <Link className='ItemLink' to={`/Item/${product.product_id}`}>
            <li className='Items' key={index}>
                <span className='ProductName'>{product.product_name}</span>
                <span> {product.unit_of_measure_size}{product.unit_of_measure}</span>
                <span> ${product.price_sale?.toFixed(2)}</span>
            </li>
          </Link>
          )}
        </ul>
      </div>
    </div>
    );
}

export default ItemListData;

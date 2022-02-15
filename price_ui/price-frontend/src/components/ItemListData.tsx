import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux'
import { RootState, store } from '../stores/store';
import { getProduct } from '../slices/ProductBasketSlice';


function ItemListData() {
	const dispatch = useDispatch();
	const { products, filteredProducts, status } = useSelector((state: RootState) => state.products) 
  
  // update state of products on page load
	useEffect(() => {
		dispatch(getProduct())
	}, [dispatch]);

  // define types for product elements
  interface ProductType {
    product_name?: string;
    unit_of_measure_size?: number;
    unit_of_measure?: string;
    price_sale?: number;
  }
  
  return (
    <div className='ItemListContainer'>
      <div className='RefreshButtonContainer'>
        <button className='RefreshButton' onClick={() => dispatch(getProduct())}>Refresh</button>
      </div>
      <div className='ItemListData'>
        <h1>{ JSON.stringify(status) }</h1>
        <ul className='ItemDisplay'>
        { filteredProducts.map((product: ProductType, index: number) => 
            <li className='Items' key={index}>
                <span className='ProductName'>{product.product_name}</span>
                <span> {product.unit_of_measure_size}{product.unit_of_measure}</span>
                <span> ${product.price_sale?.toFixed(2)}</span>
            </li>
          )}
        </ul>
      </div>
    </div>
    );
}

export default ItemListData;

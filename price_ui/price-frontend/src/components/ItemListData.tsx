import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux'
import { RootState, store } from '../stores/store';
import { getProduct } from '../slices/ProductApiSlice';


const splitString = (stringToSplit: string) => {
  let storeName = stringToSplit.split(' ')[0];
  let productInfo = stringToSplit.slice(storeName.length, stringToSplit.indexOf('$'));
  let productPrice = stringToSplit.slice(stringToSplit.indexOf('$'));
  return [storeName, productInfo, productPrice];
}



function ItemListData() {
	const dispatch = useDispatch();
	const { products, status } = useSelector((state: RootState) => state.products) 
  
	useEffect(() => {
		dispatch(getProduct())
	}, [dispatch]);

  // parse products into usable format
  let productList = JSON.parse(JSON.stringify(products));
  // check if correct state of array has updated and converted needed info into array
  if (Array.isArray(productList)){
    productList = productList.map(product => (product['store_name'] + ' ' + product['product_name'] + ' ' + product['unit_of_measure_size'] + product['unit_of_measure'] + 
    ' ' + '$' + product['price_sale']));
    // console.log(productList)
  };

  return (
    <div className='ItemListContainer'>
      <div className='RefreshButtonContainer'>
        <button className='RefreshButton' onClick={() => dispatch(getProduct())}>Refresh</button>
      </div>
      <div className='ItemListData'>
        <h1>{ JSON.stringify(status) }</h1>
        {/* only render productlist if it is an array */}
        <ul className='ItemDisplay'>
        { Array.isArray(productList) && 
          productList.map((product, index) => 
            <li className='Items' key={index}>
              <span>{splitString(product)[0]}</span>
              <span>{splitString(product)[1]}</span>
              <span>{splitString(product)[2]}</span>
            </li>
          )}
          {console.log(productList)}
        </ul>
      </div>
    </div>
    );
}

export default ItemListData;

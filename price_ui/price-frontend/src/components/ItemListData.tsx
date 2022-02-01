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

  let jsonString = JSON.parse(JSON.stringify(products));

  console.log(jsonString[0]['product_name'])

  return (
    <div>
      <div className='Refresh'>
        <button className='RefreshButton' onClick={() => dispatch(getProduct())}>
          refresh
        </button>
      </div>
        <div>
          <h2>{ JSON.stringify(products[0]) }</h2>
        </div>
    </div>
    );
}

export default ItemListData;

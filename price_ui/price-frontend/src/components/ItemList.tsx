import React from 'react';
import '../css/ItemList.css'
import ItemListData from './ItemListData';
import { getProduct } from '../slices/ProductApiSlice';
import { useDispatch } from 'react-redux'

function ItemList() {
	const dispatch = useDispatch();

  return (
    <div className='ItemContainer'>
      
      <ItemListData />
    </div>
  );
}

export default ItemList;

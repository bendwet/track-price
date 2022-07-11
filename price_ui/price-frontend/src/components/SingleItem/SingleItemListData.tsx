import React, { useLayoutEffect, useEffect } from 'react';
import { useSelector, useDispatch } from "react-redux";
import { RootState } from '../../stores/store';
import { getProductById } from '../../slices/SingleItemSlice';
import SingleItem from '../Models/SingleItem';
import { useParams } from 'react-router-dom';
import { reset } from '../../slices/SingleItemSlice';


export default function SingleItemListData() {
	
	const { singleProduct } = useSelector((state: RootState) => state.singleProduct);

  return (
    <div className='SingleProductContainer'>
			<ul className='SingleItemDisplay'>
				{ singleProduct.map((item: SingleItem, index: number) => 
					<li className='SingleProduct' key={index}>
						<span className='SingleProductStoreName'>{item.storeName} </span>
						<span className='SingleProductName'>{item.productName}</span>
						<span className='SingleProductQuantity'> {item.priceQuantity}</span>
						<span className='SingleProductPrice'> ${item.salePrice?.toFixed(2)}</span>
					</li>
				)}
			</ul>
		</div>
  );
}

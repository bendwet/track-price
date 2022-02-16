import React, { useEffect } from 'react';
import { useSelector, useDispatch } from "react-redux";
import { RootState } from '../stores/store';
import { getProductById } from '../slices/SingleItemSlice';
import ProductModel from './ProductModel';
import { useParams } from 'react-router-dom';


export default function SingleItemListData() {

	const dispatch = useDispatch();
	const { singleProduct } = useSelector((state: RootState) => state.singleItem)
	const { productId } = useParams(); 
	
	// update state of products on page load
	useEffect(() => {
		dispatch(getProductById(productId as string))
		}, [dispatch]);

	// get product id from url
  return (
    <div className='SingleItemContainer'>
			{ singleProduct.map((product: ProductModel, index: number) => 
				<li className='SingleItemList' key={index}>
					<span className='ItemStoreName'>{product.store_name} </span>
					<span className='ItemName'>{product.product_name}</span>
					<span> {product.unit_of_measure_size}{product.unit_of_measure}</span>
					<span> ${product.price_sale?.toFixed(2)}</span>
				</li>
			)}
		</div>
  );
}

import React, { useEffect } from 'react';
import { useSelector, useDispatch } from "react-redux";
import { RootState } from '../stores/store';
import { getProductById } from '../slices/SingleItemSlice';
import ProductModel from './ProductModel';
import { useParams } from 'react-router-dom';


export default function SingleItemListData() {

	const dispatch = useDispatch();
	const { singleProduct } = useSelector((state: RootState) => state.singleProduct)
	const { productId } = useParams(); 
	
	// TODO: do not show previos state
	// update state of products on page load
	useEffect(() => {
		dispatch(getProductById(productId as string))
		}, [dispatch]);

	// get product id from url
  return (
    <div className='SingleProductContainer'>
			{ singleProduct.map((product: ProductModel, index: number) => 
				<li className='SingleProductList' key={index}>
					<span className='SingleProductStoreName'>{product.store_name} </span>
					<span className='SingleProductName'>{product.product_name}</span>
					<span> {product.unit_of_measure_size}{product.unit_of_measure}</span>
					<span> ${product.price_sale?.toFixed(2)}</span>
				</li>
			)}
		</div>
  );
}

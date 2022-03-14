import React, { useLayoutEffect, useEffect } from 'react';
import { useSelector, useDispatch } from "react-redux";
import { RootState } from '../../stores/store';
import { getProductById } from '../../slices/SingleItemSlice';
import ProductModel from '../ProductModel';
import { useParams } from 'react-router-dom';
import { reset } from '../../slices/SingleItemSlice';


export default function SingleItemListData() {
	
	const { singleProduct } = useSelector((state: RootState) => state.singleProduct);

  return (
    <div className='SingleProductContainer'>
			<ul className='SingleItemDisplay'>
				{ singleProduct.map((product: ProductModel, index: number) => 
					<li className='SingleProduct' key={index}>
						<span className='SingleProductStoreName'>{product.store_name} </span>
						<span className='SingleProductName'>{product.product_name}</span>
						<span className='SingleProductQuantity'> {product.unit_of_measure_size}{product.unit_of_measure}</span>
						<span className='SingleProductPrice'> ${product.price_sale?.toFixed(2)}</span>
					</li>
				)}
			</ul>
		</div>
  );
}

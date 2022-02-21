import SingleItemChart from "./SingleItemChart";
import SingleItemProperties from "./SingleItemProperties";
import { useLayoutEffect } from 'react';
import { getProductById } from '../../slices/SingleItemSlice';
import { useDispatch } from "react-redux";
import { useParams } from 'react-router-dom';
import { reset } from '../../slices/SingleItemSlice';

export default function SingleItemInfo() {

	const dispatch = useDispatch();
	const { productId } = useParams(); 

	useLayoutEffect(() => {
		dispatch(reset())
		dispatch(getProductById(productId as string))
		}, [dispatch]);

  return (
    <div>
			<header className='SingleItemHeader'>
				<div className='SingleItemImageContainer'>
					<img className='SingleItemImage' alt='None'/>
				</div>
				<div className='SingleItemPropsContainer'> 
					<SingleItemProperties/>
				</div>
				<div className='SingleItemChartContainer'> 
					<SingleItemChart/>
				</div>
			</header>
		</div>
  );
}

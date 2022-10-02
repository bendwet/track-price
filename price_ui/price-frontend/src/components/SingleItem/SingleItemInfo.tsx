import SingleItemChart from "./SingleItemChart";
import SingleItemProperties from "./SingleItemProperties";
import { useLayoutEffect } from 'react';
import { getItemById } from '../../slices/SingleItemSlice';
import { useDispatch } from "react-redux";
import { useParams } from 'react-router-dom';

export default function SingleItemInfo() {

	const dispatch = useDispatch();
	const { productId } = useParams(); 

	useLayoutEffect(() => {
		dispatch(getItemById(productId as string))
		}, [dispatch, productId]);

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

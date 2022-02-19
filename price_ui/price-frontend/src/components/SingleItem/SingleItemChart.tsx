import { VictoryChart, VictoryArea, VictoryGroup } from 'victory';
import { getPriceById } from '../../slices/SingleItemChartSlice';
import { reset } from '../../slices/SingleItemChartSlice';
import { useParams } from 'react-router-dom';
import { useLayoutEffect } from 'react';
import ProductModel from '../ProductModel';
import { RootState } from '../../stores/store';
import { useSelector, useDispatch } from 'react-redux';


export default function SingleItemChart() {
	const dispatch = useDispatch();
	const { productId } = useParams();
	const { lowestPriceHistory } = useSelector((state: RootState) => state.lowestPriceHistory)

	useLayoutEffect(() => {
		dispatch(reset())
		dispatch(getPriceById(productId as string))
	}, [dispatch]);

  return (
    <div className='SingleItemChart'>
			{console.log(lowestPriceHistory)}
			<VictoryChart padding={{top: 0, bottom: 0, right: 0, left: 0}} width={660} height={300} domainPadding={{y: [0, 10]}}>
				<VictoryArea style={{ 
					data: { fill: "#0c536f", fillOpacity: 0.6, stroke: "#3896bb", strokeWidth: 5 } }} 
				data={lowestPriceHistory}>
				</VictoryArea>
			</VictoryChart>
		</div>
  )
}

import { VictoryChart, VictoryArea, VictoryGroup, VictoryTooltip, VictoryVoronoiContainer } from 'victory';
import { getLowestPriceItemPerDate } from '../../slices/SingleItemChartSlice';
import { reset } from '../../slices/SingleItemChartSlice';
import { useParams } from 'react-router-dom';
import { useLayoutEffect } from 'react';
import LowestPriceHistoryItem from '../Models/LowestPriceHistoryItem';
import { RootState } from '../../stores/store';
import { useSelector, useDispatch } from 'react-redux';


export default function SingleItemChart() {
	const dispatch = useDispatch();
	const { productId } = useParams();
	const { lowestPriceHistory } = useSelector((state: RootState) => state.lowestPriceHistory)

	useLayoutEffect(() => {
		dispatch(reset())
		dispatch(getLowestPriceItemPerDate(productId as string))
	}, [dispatch]); 

  return (
    <div className='SingleItemChart'>
			<VictoryGroup 
				padding={{top: 0, bottom: 0, right: 0, left: 0}} 
				width={660} height={300} 
				domainPadding={{y: [0, 5]}}
				containerComponent={
					<VictoryVoronoiContainer
						// remove extra info from date time
						labels={({ datum }: {datum: LowestPriceHistoryItem}) => `${(datum.priceDate).split("T")[0]} \n $${datum.salePrice.toFixed(2)}`}
						labelComponent={
						<VictoryTooltip 
							constrainToVisibleArea
						/>}
					/>
				}
			>	
				<VictoryArea style={{ 
					data: { fill: "#126282", fillOpacity: 0.4, stroke: "#39a5d0", strokeWidth: 4 } ,
					labels: {fontSize: 30}
				}} 
					data={lowestPriceHistory}
					x="priceDate"
					y="salePrice"
				/>
			</VictoryGroup>
		</div>
  )
}

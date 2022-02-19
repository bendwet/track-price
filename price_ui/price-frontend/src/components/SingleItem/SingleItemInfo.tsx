import SingleItemChart from "./SingleItemChart";

export default function SingleItemInfo() {
  return (
    <div>
			<header className='SingleItemHeader'>
				<div className='SingleItemImageContainer'>
					<img className='SingleItemImage' alt='None'/>
				</div>
				<div className='SingleItemTitleContainer'> 
					<h1 className='SingleItemTitle'>
						Placholder Item Info
					</h1>
				</div>
				<div className='SingleItemChartContainer'> 
					<SingleItemChart/>
				</div>
			</header>
		</div>
  );
}

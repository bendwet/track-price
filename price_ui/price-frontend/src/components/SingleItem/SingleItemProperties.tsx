import { useSelector } from "react-redux";
import { RootState } from '../../stores/store';
import Item from '../Models/Item';

export default function SingleItemProperties() {

  const { selectedItem } = useSelector((state: RootState) => state.singleItem);
	
  return (
    <div>
      <div className='SingleItemTitle'>
       <h1>{selectedItem.productName}</h1>
      </div>
      <div className='SinleItemProperties'>
        <ul>
          <li key='Quantity'>
            Quantity: {selectedItem.priceQuantity}
          </li>
        </ul>
      </div>
    </div>
  )
}

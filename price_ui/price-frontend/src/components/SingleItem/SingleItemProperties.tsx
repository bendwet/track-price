import { useSelector } from "react-redux";
import { RootState } from '../../stores/store';
import ProductModel from '../ProductModel';

export default function SingleItemProperties() {

  const { selectedProduct } = useSelector((state: RootState) => state.singleProduct);
	
  return (
    <div>
      <div className='SingleItemTitle'>
       <h1>{selectedProduct.productName}</h1>
      </div>
      <div className='SinleItemProperties'>
        <ul>
          <li key='Quantity'>
            Quantity: {selectedProduct.priceQuantity}
          </li>
        </ul>
      </div>
    </div>
  )
}

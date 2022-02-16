import '../css/Item.css';
import SingleItemList from '../components/SingleItemList';

export default function Item() {
  return (
    <div className='ItemContainer'>
      <div className='SingleItemListContainer'>
        <SingleItemList/>
      </div>
    </div>
  )
}

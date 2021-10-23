import './CartPosition.css'

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTimes } from '@fortawesome/free-solid-svg-icons'

export default function CartPosition({ cartData, onDelete }) {
  if (!cartData) {
    throw new Error("Cart data is not provided")
  }

  const { id, product, quantity } = cartData

  const image = product.images[0]

  return (
    <div className="cart-position">
      <div className="remove-button-wrapper">
        <button className="remove-button" onClick={() => onDelete(id)}><FontAwesomeIcon icon={faTimes} /></button>
      </div>
      { image ? <img src={image.url} alt={product.description} /> : <div className="placeholder-small"></div> }
      <div>
        <label>{product.title}</label>
        <label>Title</label>
      </div>
      <div className="price">
        <label>{product.price}</label>
        <label>Price</label>
      </div>
      <div>
        <label>{quantity}</label>
        <label>Quantity</label>
      </div>
    </div>
  )
}

import { useState, useEffect } from 'react'

import { useHistory } from 'react-router-dom'

import { CartRequests } from '../../Services/Request'

import CartPosition from '../../Components/CartPosition'

import './CartPage.css'

function CartPage({ host, authentication }) {

  const [json, setJson] = useState()
  const [error, setError] = useState()
  const [loaded, setLoaded] = useState()

  const history = useHistory()

  const { user, token } = authentication

  const errorMessage = <p style={{color: '#F00'}}>Error loading orders: {error}</p>
  const loadingMessage = <p>Loading cart...</p>

  const redirectToProducts = () => {
    history.push('/Products')
  }

  useEffect(() => {
    if (!loaded && !error) {
      CartRequests.Get(json => {
        setJson(json)
        setLoaded(true)
      }, error => {
        console.error(error)
        setError(error)
      }, host, { id: user.id, token })
    }
  }, [loaded, error, host, user.id, token])

  if (!host) {
    throw new Error("Host is not provided")
  }

  if (!authentication) {
    throw new Error("Authentication is not provided")
  }

  const isEmpty = () => {
    if (Array.isArray(json)) {
      return !(json.length > 0)
    }
    return true
  }

  const deleteCartPosition = (id) => {
    CartRequests.Delete(() => {
      const nextJson = json.filter(item => item.id !== id)
      if (nextJson.length > 0) {
        setJson(nextJson)
      } else {
        setJson()
      }
    }, error => {
      console.log(error)
      setError(error)
    }, host, { id: user.id, token, data: { cartPositionId: id } })
  }

  const clearCart = () => {
    CartRequests.DeleteAll(() => {
      setJson()
    }, error => {
      console.log(error)
      setError(error)
    }, host, { id: user.id, token })
  }

  const cartPositions = (json && Array.isArray(json)) && json.map((item, id) => {
    return <CartPosition key={id} cartData={item} onDelete={deleteCartPosition} />
  })

  return (
    <div className="cart-page-container">
      { !isEmpty(json) ? (
        <div className="title">
          <h1>You have {json.length} items in cart</h1>
          <button className="default-button">Buy</button>
          <button className="invert-button red-invert-button" onClick={clearCart}>Clear All</button>
        </div>
      ) : ( 
        ( loaded && <div className="title-empty">
          <h1>Your cart seems to be empty :(</h1>
          <label>Let's add some products to it!</label>
          <button className="default-button" onClick={redirectToProducts}>View products</button>
        </div> )
      ) }
      <div className="cart-positions-wrapper">
        { loaded && cartPositions }
      </div>
      { !loaded && loadingMessage }
      { error && errorMessage }

    </div>
  )
}

export default CartPage
import { useState, useEffect } from 'react'

import { ShippingInfoRequest as Request } from '../../Services/Request'

import './ShippingInfoPage.css'

const ShippingInfoProps = {
  country: '',
  city: '',
  address: '',
  postalCode: '',
  contactPhone: ''
}

function ShippingInfoPage({ authentication, host }) {

  if (!host) {
    throw new Error("Host prop is not provided")
  }

  const { user, token } = authentication
  const [editing, setEditing] = useState(false)

  const [json, setJson] = useState()
  const [data, setData] = useState()

  const [loaded, setLoaded] = useState(false)
  const [error, setError] = useState()
  const [info, setInfo] = useState()

  const fetchShippingData = () => {
    if (!token) {
      throw new Error("User token is missing")
    }
    Request.get(json => {
      setJson(json)
      setLoaded(true)
    }, error => {
      setError(error)
      console.error(error)
    }, host, { id: user.id , token })
  }

  const postShippingData = () => {
    Request.post(result => {
      setInfo(result.message)
      setEditing(false)
      setJson(data)
    }, error => {
      console.log(error)
      setError(error)
    }, host, { id: user.id, token, data })
  }

  useEffect(() => {
    if (!loaded && !error) {
      fetchShippingData()
    }
  })

  const fullfilled = data && Object.values(data).every(val => val && val.length > 0)

  // response with ShippingInfo from API
  // must contain full ShippingInfo resource
  // (with all keys)

  const missingShippingInfo = () => !(json && Object.keys(ShippingInfoProps).every(prop => Object.keys(json).includes(prop)))

  const errorMessage = <p style={{color: '#FF0000'}}>Error loading shipping data: {JSON.stringify(error)}</p>

  const loadingMessage = <p>Loading shipping data...</p>

  const updateData = (data) => {
    setData(prevData => {
      return { ...prevData, ...data }
    })
  }

  const countryChanged = ({ target }) => {
    updateData({ country: target.value })
  }

  const cityChanged = ({ target }) => {
    updateData({ city: target.value })
  }

  const addressChanged = ({ target }) => {
    updateData({ address: target.value })
  }

  const postalCodeChanged = ({ target }) => {
    updateData({ postalCode: target.value })
  }

  const contactPhoneChanged = ({ target }) => {
    updateData({ contactPhone: target.value })
  }

  const infoMessage = ( <div className="info">{info}</div> )

  return (
    <div className="shipping-info-page-container">
      <div className="shipping-info-wrapper">
        { loaded && (
          ( !missingShippingInfo() || editing ?
            <div className="data">
              <div>
                <label>Country</label>
                { editing ? 
                  <input type="text" onChange={countryChanged} value={data.country ? data.country : ''} /> : 
                  <label>{json.country}</label> 
                }
              </div>
              <div>
                <label>City</label>
                { editing ? 
                  <input type="text" onChange={cityChanged} value={data.city ? data.city : ''} /> : 
                  <label>{json.city}</label> 
                }
              </div>
              <div>
                <label>Address</label>
                { editing ? 
                  <input type="text" onChange={addressChanged} value={data.address ? data.address : ''} /> : 
                  <label>{json.address}</label> 
                }
              </div>
              <div>
                <label>Postal Code</label>
                { editing ? 
                  <input type="text" onChange={postalCodeChanged} value={data.postalCode ? data.postalCode : ''} /> : 
                  <label>{json.postalCode}</label> 
                }
              </div>
              <div>
                <label>Contact Phone</label>
                { editing ? 
                  <input type="text" onChange={contactPhoneChanged} value={data.contactPhone ? data.contactPhone : ''} /> : 
                  <label>{json.contactPhone}</label> 
                }
              </div>
            </div>
          :
            <div>
              <h2>There's no shipping info.</h2>
              <button onClick={() => {
                setEditing(true)
                setData({ ...ShippingInfoProps })
              }} className="default-button">Add one</button>
            </div> )
        ) }
        { (loaded && (!missingShippingInfo() || editing )) && <button className="default-button" 
          onClick={() => {
            if (!editing) {
              setData(json)
              setEditing(true)
            } else {
              postShippingData()
            }
          }} disabled={!fullfilled && editing}>
            { editing ? "Save" : "Edit" }
          </button> 
        }
        { editing && <button className="invert-button" onClick={() => {
          setEditing(false)
          setData(json)
        }}>Cancel</button> } 
        { error && errorMessage }
        { !loaded && loadingMessage }
        { info && infoMessage }
      </div>
      
    </div>
  )
}

export default ShippingInfoPage

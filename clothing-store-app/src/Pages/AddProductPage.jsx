import { useState, useEffect, useRef } from 'react'

import { useHistory } from 'react-router-dom'

import { AddProductRequest, BaseDataRequest } from '../Services/Request'

import './AddProductPage.css'

export default function AddProductPage({ authentication, host, onInfo, onError }) {
  if (!host) {
    throw new Error("Host is not provided")
  }

  if (!authentication) {
    throw new Error("Authenticaiton is not provided")
  }

  const { user, token } = authentication

  const history = useHistory()

  const addButtonClicked = () => {
    AddProductRequest(message => {
      onInfo("Add product", "Added product. " + message)
    }, error => {
      onError("Error", error)
    }, host, { id: user.id, token, image: state.image, data: state })
  }

  const cancelClicked = () => {
    history.goBack()
  }

  const [state, setState] = useState()

  const [loaded, setLoaded] = useState(false)

  const [baseData, setBaseData] = useState()

  const requestCancelled = useRef(false)

  const validate = () => {
    if (state) {
      return (
      (state.title && state.title.length > 0) &&
      (state.description && state.description.length > 0) &&
      (state.price && state.price > 0) &&
      (state.available && state.available > 0) &&
      (state.hasOwnProperty('typeId')) &&
      (state.hasOwnProperty('sizeId')) &&
      (state.hasOwnProperty('materialId')) &&
      (state.image)
      )
    }
  }

  useEffect(() => {
    BaseDataRequest(data => {
      if (!requestCancelled.current) {
        setBaseData(data)
        setLoaded(true)
      }
    }, error => {
      console.error(error)
      onError(error)
    }, host)

    return () => {
      requestCancelled.current = true
    }
  }, [host, onError])

  const updateState = (data) => {
    setState(prevState => {
      return { ...prevState, ...data }
    })
  }

  const fileChanged = ({ target }) => {
    let image = new FormData()
    image.append('file', target.files[0])
    updateState({ image })
  }

  const titleChanged = ({ target }) => {
    updateState({ title: target.value })
  }

  const descriptionChanged = ({ target }) => {
    updateState({ description: target.value })
  }

  const priceChanged = ({ target }) => {
    updateState({ price: parseInt(target.value) })
  }

  const availableChanged = ({ target }) => {
    updateState({ available: parseInt(target.value) })
  }

  const typeChanged = ({ target }) => {
    updateState({ typeId: parseInt(target.value) })
  }

  const sizeChanged = ({ target }) => {
    updateState({ sizeId: parseInt(target.value) })
  }

  const materialChanged = ({ target }) => {
    updateState({ materialId: parseInt(target.value) })
  }
  

  return (
    <div className="add-product-page-container">
      { loaded && 
      <div className="add-product-form">
        <div className="title-container">
          <h1>Add product</h1>
        </div>
        <div>
          <input type="file" onChange={fileChanged} />
          <label>Product image</label>
        </div>

        <div>
          <input type="text" onChange={titleChanged} />
          <label>Title</label>
        </div>

        <div>
          <input type="text" onChange={descriptionChanged} />
          <label>Description</label>
        </div>

        <div>
          <input type="number" min={0} onChange={priceChanged} />
          <label>Price</label>
        </div>

        <div>
          <input type="number" min={0} onChange={availableChanged} />
          <label>Available</label>
        </div>

        <div>
          <label>Category</label>
          <select onChange={typeChanged}>
            { 
              baseData.types.productTypes && 
              baseData.types.productTypes.map((type, id) => {
                return <option key={id} value={type.id}>{type.name}</option>
            }) }
          </select>
        </div>

        <div>
          <label>Size</label>
          <select onChange={sizeChanged}>
            { 
              baseData.sizes.productSizes && 
              baseData.sizes.productSizes.map((size, id) => {
                return <option key={id} value={size.id}>{size.name}</option>
            }) }
          </select>
        </div>

        <div>
          <label>Material</label>
          <select onClick={materialChanged} onChange={materialChanged}>
            { 
              baseData.materials.productMaterials && 
              baseData.materials.productMaterials.map((material, id) => {
                return <option key={id} value={material.id}>{material.title}</option>
            }) }
          </select>
        </div>
        <div>
          <button className="default-button" disabled={!validate()} onClick={addButtonClicked}>Add</button>
          <button className="invert-button" onClick={cancelClicked}>Cancel</button>
        </div>

    </div> }
    </div>
  )
}
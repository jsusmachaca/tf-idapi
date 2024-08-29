process.loadEnvFile() // uncomment in development mode
import express from 'express'
import cors from 'cors'
import path from 'node:path'
import { getAll, searchTF } from './controller/clientController'

const app = express()

app.use(cors())
app.use(express.static(path.join('src', 'public')))
app.set('view engine', 'ejs')
app.set('views', path.join(process.cwd(), 'src', 'views'))

app.get('/', getAll)
app.get('/search/:text', searchTF)

const PORT = process.env.PORT!
app.listen(PORT, () => {
  console.log(`Server listen on http://localhost:${PORT}`)
})

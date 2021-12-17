<template>
  <v-app>
    {{games}}
    <v-card>
      <v-container fluid>
        <v-row>
          <v-col cols="10">
            <v-row align="center">
              <v-col cols="5">
                <v-autocomplete
                  v-model="selectedName"
                  :items="games"
                  item-value="name"
                  item-text="name"
                  dense
                  chips
                  small-chips
                  label="Name"
                  solo
                ></v-autocomplete>
              </v-col>
              <v-col cols="5">
                <v-text-field
                  v-model="selectedYear"
                  dense
                  chips
                  small-chips
                  solo
                  min="2010"
                  max="2022"
                  type="number"
                  label="Année"
                />
              </v-col>
            </v-row>
            <v-row align="center">
              <v-col cols="5">
                <v-autocomplete
                  v-model="selectedCategories"
                  :items="categories"
                  item-value="categories.id"
                  item-text="name"
                  dense
                  chips
                  small-chips
                  label="Categories"
                  multiple
                  solo
                ></v-autocomplete>
              </v-col>
              <v-col cols="5">
                <v-text-field
                  v-model="selectedPrix"
                  dense
                  chips
                  small-chips
                  solo
                  min="0"
                  max="150"
                  type="number"
                  label="Prix"
                />
              </v-col>
            </v-row>
          </v-col>
          <v-col cols="2" align="center">
            <v-btn
              color="primary"
              elevation="3"
              outlined
            >Rechercher</v-btn>
          </v-col>
        </v-row>
      </v-container>
    </v-card>
    <v-card>
      <v-card-title>
        Jeux Vidéos
        <v-spacer></v-spacer>
        <v-text-field
          v-model="search"
          append-icon="mdi-magnify"
          label="Rechercher"
          single-line
          hide-details
        ></v-text-field>
      </v-card-title>
      <v-data-table
        :headers="headers"
        :items="games"
        :search="search"
        loading=isLoading
        loading-text="Loading... Please wait"
      >
        <template
        v-slot:item="{ item }">
          <tr style="text-align:left;">
            <td>
              <span>{{item.name}}</span>
            </td>
            <td>
              <span v-if="item.date != null">{{item.date}}</span>
              <span v-if="item.releaseDate != null">{{item.releaseDate}}</span>
            </td>
            <td>
              <span v-if="item.prix != null">{{item.prix}}</span>
              <span v-if="item.price != null">{{item.price}}</span>
            </td>
            <span v-if="item.categories != null">
              <span v-for="categorie in item.categories" :key="categorie.id">
                <td>
                  <v-chip color="blue" dark v-if="item.categories.$values == null">
                    {{categorie.name}}
                    </v-chip>
                  <v-chip color="blue" dark v-else>
                    ???
                    </v-chip>                    
                </td>
              </span>
            </span>         
          </tr>
        </template>
      </v-data-table>
    </v-card>
  </v-app>
</template>
<script>
const axios = require('axios');
export default {
  name: 'App',


  data: () => ({
    search: '',
    games : [],
    categories: [],
    selectedName : "",
    selectedYear : "",
    selectedCategories : [],
    selectedPrix : "",
    headers: [
      {
        text: 'Nom',
        align: 'start',
        sortable: true,
        value: 'name',
      },
      { text: 'Date', value: 'date' },
      { text: 'Prix TTC', value: 'prix' },
      { text: 'Categories', value: 'categories' },
    ],
    apis: [
      { 
        name: 'beluga',
        url: '',
        token: ""
      },
      { 
        name: 'thikler',
        url: '',
        token: ""
      },
      { 
        name: 'bigot',
        url: '',
        token: ""
      },
      { 
        name: 'william',
        url: '',
        token: ""
      }          
    ],
    dataAuth : {}
  }),

  async mounted(){
    await this.initApisUrl()
    await this.getListGames()
    await this.getListCategories()
  },

  methods:{
      async getListGames(){
        await this.auth();
        await this.initApisUrl()
        this.apis.forEach(async api => {
          switch(api.name){
            case "beluga":
              api.url +="games";
              break;
            case "bigot":
              api.url +="games";
              break;
            case "thikler":
              api.url += "List/VideoGames/Name"
              break;
            case "william":
              api.url += "games"
              break;
            default:
              break;
          }

          let rawResult = await axios({
                  method: 'get',
                  url: api.url,
                  headers: {
                    Authorization: api.token
                  }, 
                });
          if(rawResult.data.$values != null){
            for(let game of  rawResult.data.$values){
              this.games.push(game)
            }    
          }
          else if(rawResult.data.result != null){
            for(let game of  rawResult.data.result){
              this.games.push(game)
            }    
          }
          else{
            for(let game of  rawResult.data){
              this.games.push(game)
            }    
          }           
        });
    },
    async getListCategories(){
        await this.initApisUrl()
        this.apis.forEach(async api => {
          switch(api.name){
            case "beluga":
              api.url +="categories";
              break;
            case "bigot":
              api.url +="category/all";
              break;
            default:
              break;
          }
          let rawResult = await axios({
                  method: 'get',
                  url: api.url,
                  headers: {
                    Authorization: api.token
                  }, 
            });
            for(let categorie of  rawResult.data){
              this.categories.push(categorie)
            }   
       });
    },
    async auth(){
      for (let api of this.apis) {
        switch(api.name){
          case 'beluga':
            api.url+= "auth"
            api.token = "Bearer "
            this.dataAuth = {
              username: "admin",
              password: "patafoin"
            }
            break;
          case 'bigot':
            api.url+= "users/login"
            this.dataAuth = {
              email: "quentin@live.fr",
              password: "123456aA"
            }
            break; 
          case 'thikler' :
            api.token = "Bearer "
            api.url+= "auth"
            this.dataAuth = {
              username: "Klervia",
              password: "Thibaut"
            }              
            break;
          case 'william' :
            api.url+= "signIn"
            this.dataAuth = {
              email: "cocodu53",
              password: "erwan"
            }              
            break;
          default:
            break;       
        }


        let rawResult = await axios({
            method: 'post',
            url: api.url,
            headers: {}, 
            data: this.dataAuth
        });  
        if(rawResult.data.token != null){
          api.token += rawResult.data.token 
        }
        else{
          api.token = rawResult.data.accessToken 
        }
      }      
  },

    async initApisUrl(){
      this.apis.forEach(api => {
        switch(api.name){
          case "beluga":
            api.url = "https://belugaming.herokuapp.com/api/"
            break;
          case "bigot":
            api.url = "https://bigogaming.herokuapp.com/"; 
            break;
          case "thikler":
            api.url = "https://thikler.herokuapp.com/api/"
            break;
          case "william":
            api.url = "https://bibliojvapi.herokuapp.com/api/"
            break;
          default:
            break;
        }
    });
    }
  }
};
</script>


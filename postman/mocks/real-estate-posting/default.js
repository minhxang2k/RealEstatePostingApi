const http = require('http');
const PORT = process.env.PORT || 5294;

const mockPosts = [
  {
    id: 7,
    title: "Modern Villa in District 2",
    content: "A beautiful modern villa with spacious living areas and a private pool, perfect for families.",
    address: "12342321/212 skibidi sigma",
    latitude: "12.123",
    longitude: "21.432",
    price: 20000000,
    city_Id: 1,
    district_Id: 1,
    ward_Id: 1,
    street_Id: 1,
    property_Type_Id: 1,
    transaction_Type_Assignment_Id: 1,
    is_Recommended: false,
    status_Id: 1,
    view_Count: 42,
    created_Date: "2026-01-15T08:00:00",
    updated_Date: null,
    post_Detail: {
      square: 50,
      bedRoom: 2,
      bathRoom: 1,
      house_Direction: "North",
      facade: 5,
      total_Floors: 3
    },
    post_Pictures: [
      { id: 1, image_Url: "/images/post_7_thumb.jpg", picture_Type: "thumbnail", is_Thumbnail: true },
      { id: 2, image_Url: "/images/post_7_pic1.jpg", picture_Type: "picture", is_Thumbnail: false }
    ],
    post_Video: { id: 1, video_Url: "/videos/post_7.mp4" }
  },
  {
    id: 8,
    title: "Cozy Apartment near City Center",
    content: "A cozy 2-bedroom apartment within walking distance to the city center, shops, and restaurants.",
    address: "45 Nguyen Hue, District 1",
    latitude: "10.776",
    longitude: "106.700",
    price: 5000000,
    city_Id: 1,
    district_Id: 2,
    ward_Id: 3,
    street_Id: 5,
    property_Type_Id: 2,
    transaction_Type_Assignment_Id: 2,
    is_Recommended: true,
    status_Id: 1,
    view_Count: 120,
    created_Date: "2026-02-10T09:30:00",
    updated_Date: "2026-02-12T11:00:00",
    post_Detail: {
      square: 75,
      bedRoom: 2,
      bathRoom: 2,
      house_Direction: "South",
      facade: 6,
      total_Floors: 1
    },
    post_Pictures: [
      { id: 3, image_Url: "/images/post_8_thumb.jpg", picture_Type: "thumbnail", is_Thumbnail: true }
    ],
    post_Video: null
  }
];

const server = http.createServer((req, res) => {
  const { method, url } = req;

  // @endpoint GET /Posts/List
  if (method === 'GET' && url === '/Posts/List') {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify(mockPosts));
    return;
  }

  // @endpoint GET /Posts/List/{id}
  const listByIdMatch = url.match(/^\/Posts\/List\/(\d+)$/);
  if (method === 'GET' && listByIdMatch) {
    const id = parseInt(listByIdMatch[1], 10);
    const post = mockPosts.find(p => p.id === id) || null;
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify(post));
    return;
  }

  res.writeHead(404, { 'Content-Type': 'application/json' });
  res.end(JSON.stringify({ error: 'Mock route not defined', method, url }));
});

server.listen(PORT, () => console.log('Mock server running on port ' + PORT));
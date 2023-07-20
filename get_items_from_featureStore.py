import boto3

# Create a Boto3 client for FeatureStoreRuntime
client = boto3.client('sagemaker-featurestore-runtime', region_name='us-east-1')

# Specify the Feature Group and item identifier
feature_group_name = 'dse-review-sentiment-feature-group'

# Make the get_item API call
response = client.batch_get_record(
    Identifiers=[
        {
            'FeatureGroupName':feature_group_name, 
            'FeatureNames':['AVG_VADER_SENTIMENT_SCORE'], 
            'RecordIdentifiersValueAsString':[
                "75652",
                "45986",
                "45722",
                "45073",
                "44936",
                "45723",
                "49772",
                "49635",
                "54814",
                "51577",
                "45079",
                "94136",
                "105288",
                "106874",
                "114511",
                "148559",
                "45071",
                "45158",
                "50911",
                "79871",
                "50079",
                "51907",
                "86966",
                "56895",
                "89277",
                "92589",
                "93329",
                "122316",
                "131142",
                "140707",
                "143507",
                "143515",
                "196210",
                "203107",
                "46392",
                "52010",
            ],
        }
    ]
)

data = []
for m in response["Records"]:
    item = { "PartNumber": m['RecordIdentifierValueAsString'], 'Sentiment': m["Record"][0]["ValueAsString"] }
    data.append(item)

results = sorted(data, key=lambda x: x["Sentiment"], reverse=True)

for a in results:
    print(a)